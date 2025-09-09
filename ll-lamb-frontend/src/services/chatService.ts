import { Chat, Message } from '@/types/chat';

// Dummy API service for Azure .NET backend
class ChatService {
  private baseUrl = process.env.NEXT_PUBLIC_API_URL || 'https://your-azure-api.azurewebsites.net/api';

  async sendMessage(chatId: string, content: string): Promise<Message> {
    // Simulate API call delay
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    // Return dummy response
    return {
      id: Date.now().toString(),
      content: `This is a dummy response to: "${content}"`,
      role: 'assistant',
      timestamp: new Date()
    };
  }

  async createChat(): Promise<Chat> {
    // Simulate API call delay
    await new Promise(resolve => setTimeout(resolve, 500));
    
    const now = new Date();
    return {
      id: Date.now().toString(),
      title: 'New Chat',
      messages: [],
      createdAt: now,
      updatedAt: now
    };
  }

  async getChatHistory(): Promise<Chat[]> {
    // Return dummy chat history
    await new Promise(resolve => setTimeout(resolve, 300));
    
    return [];
  }

  async deleteChat(chatId: string): Promise<void> {
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 300));
    console.log(`Deleting chat: ${chatId}`);
  }
}

export const chatService = new ChatService();