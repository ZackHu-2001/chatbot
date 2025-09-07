import { useState, useEffect } from 'react';
import { Chat } from '@/types/chat';

export function useChatHistory() {
  const [chats, setChats] = useState<Chat[]>([]);

  useEffect(() => {
    // Load chat history from localStorage on component mount
    const savedChats = localStorage.getItem('chatHistory');
    if (savedChats) {
      try {
        const parsedChats = JSON.parse(savedChats).map((chat: any) => ({
          ...chat,
          createdAt: new Date(chat.createdAt),
          updatedAt: new Date(chat.updatedAt),
          messages: chat.messages.map((msg: any) => ({
            ...msg,
            timestamp: new Date(msg.timestamp)
          }))
        }));
        setChats(parsedChats);
      } catch (error) {
        console.error('Failed to parse chat history:', error);
      }
    }
  }, []);

  const saveChatHistory = (updatedChats: Chat[]) => {
    setChats(updatedChats);
    localStorage.setItem('chatHistory', JSON.stringify(updatedChats));
  };

  const addChat = (chat: Chat) => {
    const updatedChats = [chat, ...chats];
    saveChatHistory(updatedChats);
  };

  const updateChat = (chatId: string, updatedChat: Partial<Chat>) => {
    const updatedChats = chats.map(chat =>
      chat.id === chatId ? { ...chat, ...updatedChat, updatedAt: new Date() } : chat
    );
    saveChatHistory(updatedChats);
  };

  const deleteChat = (chatId: string) => {
    const updatedChats = chats.filter(chat => chat.id !== chatId);
    saveChatHistory(updatedChats);
  };

  return {
    chats,
    addChat,
    updateChat,
    deleteChat
  };
}