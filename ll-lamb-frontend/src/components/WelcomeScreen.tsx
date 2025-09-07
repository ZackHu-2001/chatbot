'use client';

import { User, Sparkles } from 'lucide-react';

interface WelcomeScreenProps {
  onCreateAgent?: () => void;
}

export function WelcomeScreen({ onCreateAgent }: WelcomeScreenProps) {
  return (
    <div className="flex-1 flex items-center justify-center p-8">
      <div className="text-center max-w-md">
        <div className="mb-8">
          <div className="w-16 h-16 bg-gradient-to-r from-purple-500 to-blue-500 rounded-2xl flex items-center justify-center mx-auto mb-4">
            <Sparkles className="w-8 h-8 text-white" />
          </div>
          <h1 className="text-4xl font-bold text-white mb-2">
            Good evening, Guest
          </h1>
          <p className="text-gray-400 text-lg">
            What would you like to do?
          </p>
        </div>

        <button
          onClick={onCreateAgent}
          className="inline-flex items-center gap-3 px-6 py-3 bg-gradient-to-r from-purple-600 to-blue-600 hover:from-purple-700 hover:to-blue-700 rounded-xl text-white font-medium transition-all transform hover:scale-105"
        >
          <User className="w-5 h-5" />
          Create Agent
        </button>
      </div>
    </div>
  );
}