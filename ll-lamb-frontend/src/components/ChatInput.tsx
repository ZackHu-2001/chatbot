'use client';

import { useState } from 'react';
import { Send, Paperclip, Mic } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ChatInputProps {
  onSendMessage: (message: string) => void;
  disabled?: boolean;
  placeholder?: string;
}

export function ChatInput({ 
  onSendMessage, 
  disabled = false, 
  placeholder = "Ask your agent..." 
}: ChatInputProps) {
  const [message, setMessage] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (message.trim() && !disabled) {
      onSendMessage(message.trim());
      setMessage('');
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSubmit(e);
    }
  };

  return (
    <div className="p-4 border-t border-gray-700/50 bg-gray-900/30 backdrop-blur-sm">
      <div className="max-w-4xl mx-auto">
        {/* Suggested Actions */}
        <div className="flex gap-2 mb-4 flex-wrap">
          <button className="flex items-center gap-2 px-3 py-2 bg-gray-800/50 hover:bg-gray-800/70 rounded-full text-sm text-gray-300 transition-colors">
            <span>🐑</span>
            Image of a working lamb
          </button>
          <button className="flex items-center gap-2 px-3 py-2 bg-gray-800/50 hover:bg-gray-800/70 rounded-full text-sm text-gray-300 transition-colors">
            <span>📰</span>
            Latest AI news
          </button>
          <button className="flex items-center gap-2 px-3 py-2 bg-gray-800/50 hover:bg-gray-800/70 rounded-full text-sm text-gray-300 transition-colors">
            <span>🎨</span>
            Sunset painting
          </button>
          <button className="flex items-center gap-2 px-3 py-2 bg-gray-800/50 hover:bg-gray-800/70 rounded-full text-sm text-gray-300 transition-colors">
            <span>📈</span>
            HN trending topics
          </button>
        </div>

        {/* Input Form */}
        <form onSubmit={handleSubmit} className="relative">
          <div className="relative flex items-end gap-2 bg-gray-800/50 rounded-2xl p-3 border border-gray-700/50 focus-within:border-gray-600/50 transition-colors">
            <button
              type="button"
              className="p-2 text-gray-400 hover:text-gray-300 transition-colors"
              disabled={disabled}
            >
              <Paperclip className="w-5 h-5" />
            </button>

            <textarea
              value={message}
              onChange={(e) => setMessage(e.target.value)}
              onKeyDown={handleKeyDown}
              placeholder={placeholder}
              disabled={disabled}
              className="flex-1 bg-transparent text-white placeholder-gray-400 resize-none min-h-[20px] max-h-32 outline-none"
              rows={1}
              style={{ 
                height: 'auto',
                minHeight: '20px'
              }}
              onInput={(e) => {
                const target = e.target as HTMLTextAreaElement;
                target.style.height = 'auto';
                target.style.height = Math.min(target.scrollHeight, 128) + 'px';
              }}
            />

            <div className="flex items-center gap-2">
              <button
                type="button"
                className="p-2 text-gray-400 hover:text-gray-300 transition-colors"
                disabled={disabled}
              >
                <Mic className="w-5 h-5" />
              </button>

              <button
                type="submit"
                disabled={disabled || !message.trim()}
                className={cn(
                  'p-2 rounded-lg transition-colors',
                  disabled || !message.trim()
                    ? 'text-gray-500 cursor-not-allowed'
                    : 'text-white bg-gradient-to-r from-purple-600 to-blue-600 hover:from-purple-700 hover:to-blue-700'
                )}
              >
                <Send className="w-5 h-5" />
              </button>
            </div>
          </div>

          {/* Assistant Selection */}
          <div className="flex items-center justify-between mt-2 text-sm">
            <div className="flex items-center gap-2 text-gray-400">
              <button className="flex items-center gap-1 hover:text-gray-300 transition-colors">
                <span className="w-2 h-2 bg-blue-500 rounded-full"></span>
                Default Assistant
                <span className="text-xs">▼</span>
              </button>
            </div>

            <button 
              className="text-gray-400 hover:text-gray-300 transition-colors"
              disabled={disabled}
            >
              Execute ↵
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}