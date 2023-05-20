export class User{
    id: number = 0;
    username: string = "";
}

export class Chat{
    id: number = 0;
    name: string = "";
    creationDate: string = "";
    messages: Message[] = [];
    chatParticipants: ChatParticipant[] = [];
}

export class Message{
    id: number = 0;
    text: string = "";
    timestamp: string = "";
    userId: number = 0;
    chatId: number = 0;
    user: User = new User();
    chat: Chat = new Chat();    
}

export class ChatParticipant{
    userId: number = 0;
    chatId: number = 0;
    user: User = new User();
    chat: Chat = new Chat(); 
}