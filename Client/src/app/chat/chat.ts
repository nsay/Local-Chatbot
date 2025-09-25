import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: 'chat.html',
  styleUrls: ['chat.css'],
})
export class Chat {
  messages: { sender: string; text: string; timestamp: Date }[] = [];
  newMessage: string = '';
  useProjectContext = false;

  projectPath: string = '';
  projectType: string = 'angular';
  projectJson: any = {};

  constructor(private chatService: ChatService) {}

  sendMessage() {
    if (!this.newMessage.trim()) return;

    const userMsg = { sender: 'User', text: this.newMessage, timestamp: new Date() };
    this.messages.push(userMsg);
    this.scrollToBottom();

    const userQuestion = this.newMessage;
    const projectData = this.useProjectContext ? JSON.stringify(this.projectJson) : "{}";

    this.newMessage = '';

    this.chatService.askBot(userQuestion, projectData, this.useProjectContext).subscribe({
      next: (res) => {
        this.messages.push({ sender: 'Bot', text: res.response, timestamp: new Date() });
        this.scrollToBottom();
      },
      error: () => {
        this.messages.push({ sender: 'Bot', text: 'Error contacting LLM.', timestamp: new Date() });
        this.scrollToBottom();
      },
    });
  }

  loadProject(path: string, type: string) {
    if (!path.trim()) return;

    this.chatService.getProjectContent(path, type).subscribe({
      next: (res) => {
        this.projectJson  = res;

        this.messages.push({
          sender: 'Bot',
          text: `Project loaded successfully!`,
          timestamp: new Date(),
        });
        this.scrollToBottom();
      },
      error: () => {
        this.messages.push({
          sender: 'Bot',
          text: 'Error loading project.',
          timestamp: new Date(),
        });
        this.scrollToBottom();
      },
    });
  }

  clearMessages() {
    this.messages = [];
  }

  scrollToBottom() {
    setTimeout(() => {
      const container = document.querySelector('.messages');
      if (container) container.scrollTop = container.scrollHeight;
    }, 0);
  }
}
