This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 20.3.2.

## Functionality

- Fully functional with user and bot messages

- Scrollable messages area only

- Auto-scroll on new messages

- Avatars (U/B) and colored bubbles

- Timestamps with good contrast

- Multi-line messages and file lists with proper formatting

## Workflow

User input (Angular UI) 

&darr;

Backend receives message + project JSON

&darr;

Backend sends data + user query to local LLM

&darr;

LLM generates natural language response
 
&darr;

Backend returns text to frontend

&darr;

Chat UI displays the AI response

## Angular Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Dotnet Development server

To start a local development server, run:

```bash
dotnet run
```

Once the server is running, open your browser and navigate to `http://localhost:5057/`. The application will automatically reload whenever you modify any of the source files.