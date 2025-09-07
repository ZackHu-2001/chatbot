# LL-Lamb Chat Frontend

A Next.js-based chat application that replicates the LL-Lamb interface with chat history, real-time messaging, and Azure .NET backend integration.

## Features

- 🎨 Modern UI matching the original LL-Lamb design
- 💬 Real-time chat interface
- 📝 Chat history with local storage
- 🔄 Sidebar with collapsible navigation
- 🎯 Responsive design
- 🚀 Ready for Azure .NET backend integration

## Getting Started

### Prerequisites

- Node.js 18+ 
- npm or yarn

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd ll-lamb-frontend
```

2. Install dependencies:
```bash
npm install
```

3. Configure environment variables:
```bash
cp .env.local.example .env.local
# Edit .env.local with your Azure API URL
```

4. Run the development server:
```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) to see the application.

## Architecture

### Frontend Components

- **Sidebar**: Navigation with chat history and controls
- **ChatMessage**: Individual message component for user/assistant messages  
- **ChatInput**: Message input with suggested actions
- **WelcomeScreen**: Initial landing page matching the original design

### Data Management

- **useChatHistory**: Custom hook for managing chat history with localStorage
- **chatService**: Service layer for API communication
- **Types**: TypeScript definitions for Chat and Message objects

### Backend Integration

The application is designed to work with a .NET backend deployed on Azure. The API service includes:

- `POST /api/chat` - Create new chat
- `POST /api/chat/{id}/messages` - Send message
- `GET /api/chats` - Get chat history
- `DELETE /api/chat/{id}` - Delete chat

## Dummy API

Currently uses a dummy API service that simulates backend responses. Replace with actual Azure .NET API endpoints by updating `src/services/chatService.ts`.

## Styling

- Built with Tailwind CSS
- Dark theme with gradient backgrounds
- Glass morphism effects
- Responsive design

## Development

```bash
npm run dev      # Start development server
npm run build    # Build for production
npm run start    # Start production server  
npm run lint     # Run ESLint
```

## Deployment

Ready for deployment to Vercel, Netlify, or any static hosting platform.

For Azure Static Web Apps:
1. Build the project: `npm run build`
2. Deploy the `out` folder to Azure Static Web Apps
3. Configure API backend URL in environment variables
