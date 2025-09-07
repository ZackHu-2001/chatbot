'use client';

import { useState, useEffect, useRef } from 'react';
import { Sidebar } from '@/components/Sidebar';
import { ChatMessage } from '@/components/ChatMessage';
import { ChatInput } from '@/components/ChatInput';
import { WelcomeScreen } from '@/components/WelcomeScreen';
import { useChatHistory } from '@/hooks/useChatHistory';
import { chatService } from '@/services/chatService';
import { Chat, Message } from '@/types/chat';

export default function Home() {
  const [currentChat, setCurrentChat] = useState<Chat | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const { chats, addChat, updateChat, deleteChat } = useChatHistory();

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [currentChat?.messages]);

  const handleNewChat = async () => {
    try {
      const newChat = await chatService.createChat();
      addChat(newChat);
      setCurrentChat(newChat);
    } catch (error) {
      console.error('Failed to create new chat:', error);
    }
  };

  const handleSelectChat = (chatId: string) => {
    const chat = chats.find(c => c.id === chatId);
    if (chat) {
      setCurrentChat(chat);
    }
  };

  const handleDeleteChat = (chatId: string) => {
    deleteChat(chatId);
    if (currentChat?.id === chatId) {
      setCurrentChat(null);
    }
  };

  const handleSendMessage = async (content: string) => {
    if (!currentChat || isLoading) return;

    const userMessage: Message = {
      id: Date.now().toString(),
      content,
      role: 'user',
      timestamp: new Date()
    };

    // Update chat with user message
    const updatedMessages = [...currentChat.messages, userMessage];
    const updatedChat = { 
      ...currentChat, 
      messages: updatedMessages,
      title: currentChat.messages.length === 0 ? content.slice(0, 50) + '...' : currentChat.title
    };
    
    setCurrentChat(updatedChat);
    updateChat(currentChat.id, updatedChat);

    setIsLoading(true);

    try {
      // Send to API and get response
      const assistantMessage = await chatService.sendMessage(currentChat.id, content);
      
      // Update chat with assistant response
      const finalMessages = [...updatedMessages, assistantMessage];
      const finalChat = { ...updatedChat, messages: finalMessages };
      
      setCurrentChat(finalChat);
      updateChat(currentChat.id, finalChat);
    } catch (error) {
      console.error('Failed to send message:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex h-screen bg-gradient-to-br from-gray-900 via-gray-800 to-gray-900">
      {/* Sidebar */}
      <Sidebar
        chats={chats}
        currentChatId={currentChat?.id}
        onNewChat={handleNewChat}
        onSelectChat={handleSelectChat}
        onDeleteChat={handleDeleteChat}
        isCollapsed={sidebarCollapsed}
        onToggleCollapse={() => setSidebarCollapsed(!sidebarCollapsed)}
      />

      {/* Main Chat Area */}
      <div className="flex-1 flex flex-col">
        {currentChat ? (
          <>
            {/* Chat Header */}
            <div className="flex items-center justify-between p-4 border-b border-gray-700/50 bg-gray-900/30 backdrop-blur-sm">
              <div className="flex items-center gap-3">
                <h2 className="text-lg font-semibold text-white truncate">
                  {currentChat.title}
                </h2>
              </div>
              <div className="flex items-center gap-2">
                <span className="text-sm text-gray-400">Auto</span>
                <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              </div>
            </div>

            {/* Messages */}
            <div className="flex-1 overflow-y-auto">
              <div className="max-w-4xl mx-auto">
                {currentChat.messages.map((message) => (
                  <ChatMessage key={message.id} message={message} />
                ))}
                {isLoading && (
                  <div className="flex gap-4 p-4">
                    <div className="w-8 h-8 rounded-full bg-gradient-to-r from-purple-500 to-blue-500 flex items-center justify-center flex-shrink-0">
                      <span className="text-white text-sm font-medium">AI</span>
                    </div>
                    <div className="max-w-3xl rounded-2xl px-4 py-3 bg-gray-800/50 text-gray-100">
                      <div className="flex items-center gap-2">
                        <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce"></div>
                        <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '0.1s' }}></div>
                        <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '0.2s' }}></div>
                      </div>
                    </div>
                  </div>
                )}
                <div ref={messagesEndRef} />
              </div>
            </div>

            {/* Chat Input */}
            <ChatInput 
              onSendMessage={handleSendMessage}
              disabled={isLoading}
            />
          </>
        ) : (
          /* Welcome Screen */
          <>
            <WelcomeScreen onCreateAgent={handleNewChat} />
            <ChatInput 
              onSendMessage={(message) => {
                handleNewChat().then(() => {
                  // Wait a bit for the chat to be created, then send the message
                  setTimeout(() => handleSendMessage(message), 100);
                });
              }}
              placeholder="Ask your agent..."
            />
          </>
        )}
      </div>
    </div>
  );
}
