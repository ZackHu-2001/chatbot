'use client';

import { Message } from '@/types/chat';
import { cn } from '@/lib/utils';

interface ChatMessageProps {
  message: Message;
}

export function ChatMessage({ message }: ChatMessageProps) {
  const isUser = message.role === 'user';

  return (
    <div className={cn(
      'flex gap-4 p-4 group',
      isUser ? 'justify-end' : 'justify-start'
    )}>
      {!isUser && (
        <div className="w-8 h-8 rounded-full bg-gradient-to-r from-purple-500 to-blue-500 flex items-center justify-center flex-shrink-0">
          <span className="text-white text-sm font-medium">AI</span>
        </div>
      )}
      
      <div className={cn(
        'max-w-3xl rounded-2xl px-4 py-3 break-words',
        isUser 
          ? 'bg-gradient-to-r from-purple-600 to-blue-600 text-white ml-12' 
          : 'bg-gray-800/50 text-gray-100 mr-12'
      )}>
        <p className="whitespace-pre-wrap">{message.content}</p>
        <div className={cn(
          'text-xs mt-2 opacity-60',
          isUser ? 'text-gray-200' : 'text-gray-400'
        )}>
          {message.timestamp.toLocaleTimeString([], { 
            hour: '2-digit', 
            minute: '2-digit' 
          })}
        </div>
      </div>

      {isUser && (
        <div className="w-8 h-8 rounded-full bg-gradient-to-r from-green-500 to-teal-500 flex items-center justify-center flex-shrink-0">
          <span className="text-white text-sm font-medium">U</span>
        </div>
      )}
    </div>
  );
}