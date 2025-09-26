import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * ChatService handles communication between the Angular frontend
 * and the backend APIs for chatbot interactions and project loading.
 */

@Injectable({
  providedIn: 'root'
})

export class ChatService {
  // these urls must match the endpoint in their respective backend controllers
  private chatApiUrl = 'http://localhost:5057/api/chat/ask';
  private projectApiUrl = 'http://localhost:5057/api/project/content'; 

  constructor(private http: HttpClient) {}

  // Send user's question to LLM
  askBot(userQuestion: string, projectJson: any, useProjectContext: boolean) {
    return this.http.post<{ response: string }>(
      this.chatApiUrl,
      { userQuestion, projectJson, useProjectContext } // these fields here must match backend model in ChatRequest.cs
    );
  }

  // Get full project JSON content
  getProjectContent(path: string, type: string): Observable<{ content: string }> {
    return this.http.post<{ content: string }>(this.projectApiUrl, { path, type });
  }
}
