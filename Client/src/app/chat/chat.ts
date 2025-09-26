import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../services/chat.service';

/**
 * Chat component for user interaction with the LLM (chatbot)
 */

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: 'chat.html',
  styleUrls: ['chat.css'],
})

export class Chat {
  messages: { sender: string; text: string; timestamp: Date }[] = []; // Array to hold chat messages
  newMessage: string = ''; // Holds the new message typed by the user
  useProjectContext = false; // flag to indicate mode

  // Holds the currently loaded project's path, type, and JSON content
  projectPath: string = '';
  projectType: string = 'angular';
  projectJson: any = {};

  constructor(private chatService: ChatService) {}

  /**
   * Handles sending a message from the user to the chatbot.
   * Adds the user's message to the chat window, then sends it to the backend.
   * Receives the bot's response and appends it to messages.
   */
  sendMessage() {
    if (!this.newMessage.trim()) return;

    // Create a message object for the user
    const userMsg = { sender: 'User', text: this.newMessage, timestamp: new Date() };
    this.messages.push(userMsg);
    this.scrollToBottom();

    // Prepare data to send to backend
    const userQuestion = this.newMessage;
    const projectData = this.useProjectContext ? JSON.stringify(this.projectJson) : "{}";

    this.newMessage = ''; // Clear input field

    // Call ChatService to send question to LLM
    this.chatService.askBot(userQuestion, projectData, this.useProjectContext).subscribe({
      next: (res) => {
        // Append bot's response to chat
        this.messages.push({ sender: 'Bot', text: res.response, timestamp: new Date() });
        this.scrollToBottom();
      },
      error: () => {
        // Show error message if backend fails
        this.messages.push({ sender: 'Bot', text: 'Error contacting LLM.', timestamp: new Date() });
        this.scrollToBottom();
      },
    });
  }

  /**
   * Loads a project from the given path and type.
   * Fetches project content via ChatService and stores it here.
   */
  loadProject(path: string, type: string) {
    if (!path.trim()) return;

    // Call ChatService to send file path and file type to LLM
    this.chatService.getProjectContent(path, type).subscribe({
      next: (res) => {
        this.projectJson  = res; // Save return project JSON

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

  /**
   * Clears all messages in the chat window
   * (..does not clear current project context)
   */
  clearMessages() {
    this.messages = [];
  }

  /**
   * Scrolls chat window to the bottom
   */
  scrollToBottom() {
    setTimeout(() => {
      const container = document.querySelector('.messages');
      if (container) container.scrollTop = container.scrollHeight;
    }, 0);
  }
}
