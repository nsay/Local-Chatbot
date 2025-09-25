import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chatApiUrl = 'http://localhost:5057/api/chat/ask';
  private projectApiUrl = 'http://localhost:5057/api/project/content'; 

  constructor(private http: HttpClient) {}

  // Ask LLM
  askBot(userQuestion: string, projectJson: any, useProjectContext: boolean) {
    return this.http.post<{ response: string }>(
      this.chatApiUrl,
      { userQuestion, projectJson, useProjectContext } // <-- must match backend property
    );
  }

  // Get full project content
  getProjectContent(path: string, type: string): Observable<{ content: string }> {
    return this.http.post<{ content: string }>(this.projectApiUrl, { path, type });
  }
}
