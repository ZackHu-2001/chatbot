'use client';

import { useState } from 'react';
import { Plus, MessageSquare, ChevronLeft, Settings, Trash2 } from 'lucide-react';
import { Chat } from '@/types/chat';
import { cn } from '@/lib/utils';

interface SidebarProps {
  chats: Chat[];
  currentChatId?: string;
  onNewChat: () => void;
  onSelectChat: (chatId: string) => void;
  onDeleteChat: (chatId: string) => void;
  isCollapsed?: boolean;
  onToggleCollapse?: () => void;
}

export function Sidebar({
  chats,
  currentChatId,
  onNewChat,
  onSelectChat,
  onDeleteChat,
  isCollapsed = false,
  onToggleCollapse
}: SidebarProps) {
  const [hoveredChatId, setHoveredChatId] = useState<string | null>(null);

  const formatTimeAgo = (date: Date) => {
    const now = new Date();
    const diffInMinutes = Math.floor((now.getTime() - date.getTime()) / (1000 * 60));
    
    if (diffInMinutes < 1) return 'Just now';
    if (diffInMinutes < 60) return `${diffInMinutes} minutes ago`;
    if (diffInMinutes < 1440) return `${Math.floor(diffInMinutes / 60)} hours ago`;
    return `${Math.floor(diffInMinutes / 1440)} days ago`;
  };

  return (
    <div className={cn(
      'h-screen bg-gray-900/50 backdrop-blur-sm border-r border-gray-700/50 flex flex-col transition-all duration-300',
      isCollapsed ? 'w-16' : 'w-80'
    )}>
      {/* Header */}
      <div className="flex items-center justify-between p-4 border-b border-gray-700/50">
        {!isCollapsed && (
          <>
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 bg-gradient-to-r from-purple-500 to-blue-500 rounded-lg flex items-center justify-center">
                <MessageSquare className="w-4 h-4 text-white" />
              </div>
              <h1 className="text-xl font-semibold text-white">LL-Lamb</h1>
            </div>
            <button
              onClick={onToggleCollapse}
              className="p-1 hover:bg-gray-800 rounded-lg transition-colors"
            >
              <ChevronLeft className="w-5 h-5 text-gray-400" />
            </button>
          </>
        )}
        {isCollapsed && (
          <button
            onClick={onToggleCollapse}
            className="p-2 hover:bg-gray-800 rounded-lg transition-colors mx-auto"
          >
            <MessageSquare className="w-5 h-5 text-gray-400" />
          </button>
        )}
      </div>

      {/* New Chat Button */}
      <div className="p-4">
        <button
          onClick={onNewChat}
          className="w-full flex items-center gap-3 p-3 bg-gradient-to-r from-purple-600 to-blue-600 hover:from-purple-700 hover:to-blue-700 rounded-lg transition-all text-white font-medium"
        >
          <Plus className="w-5 h-5" />
          {!isCollapsed && 'New Chat'}
        </button>
      </div>

      {/* Navigation */}
      {!isCollapsed && (
        <div className="px-4 pb-4">
          <div className="flex gap-2">
            <button className="flex items-center gap-2 px-3 py-2 bg-gray-800/50 rounded-lg text-white text-sm">
              <MessageSquare className="w-4 h-4" />
              Agents
            </button>
            <button className="flex items-center gap-2 px-3 py-2 text-gray-400 hover:text-white hover:bg-gray-800/30 rounded-lg text-sm transition-colors">
              <div className="w-4 h-4 rounded-full border border-current" />
              Memory
            </button>
          </div>
        </div>
      )}

      {/* Chat History */}
      <div className="flex-1 overflow-y-auto px-4">
        {!isCollapsed && (
          <div className="pb-4">
            <div className="flex items-center justify-between mb-3">
              <span className="text-sm font-medium text-gray-300">Recent</span>
              <span className="text-xs text-gray-500">{chats.length}</span>
            </div>
            <div className="space-y-2">
              {chats.map((chat) => (
                <div
                  key={chat.id}
                  className="relative group"
                  onMouseEnter={() => setHoveredChatId(chat.id)}
                  onMouseLeave={() => setHoveredChatId(null)}
                >
                  <button
                    onClick={() => onSelectChat(chat.id)}
                    className={cn(
                      'w-full text-left p-3 rounded-lg transition-colors relative',
                      currentChatId === chat.id 
                        ? 'bg-gray-800/70 border border-gray-600' 
                        : 'hover:bg-gray-800/30'
                    )}
                  >
                    <div className="flex items-center gap-3">
                      <div className="w-2 h-2 bg-blue-500 rounded-full" />
                      <div className="flex-1 min-w-0">
                        <p className="text-white text-sm font-medium truncate">
                          {chat.title}
                        </p>
                        <p className="text-gray-400 text-xs">
                          {formatTimeAgo(chat.updatedAt)}
                        </p>
                      </div>
                    </div>
                  </button>
                  {hoveredChatId === chat.id && (
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        onDeleteChat(chat.id);
                      }}
                      className="absolute right-2 top-1/2 -translate-y-1/2 p-1 hover:bg-gray-700 rounded opacity-0 group-hover:opacity-100 transition-opacity"
                    >
                      <Trash2 className="w-4 h-4 text-gray-400 hover:text-red-400" />
                    </button>
                  )}
                </div>
              ))}
            </div>
          </div>
        )}
      </div>

      {/* Settings */}
      <div className="p-4 border-t border-gray-700/50">
        <button className="w-full flex items-center gap-3 p-3 text-gray-400 hover:text-white hover:bg-gray-800/30 rounded-lg transition-colors">
          <Settings className="w-5 h-5" />
          {!isCollapsed && 'Settings'}
        </button>
      </div>
    </div>
  );
}