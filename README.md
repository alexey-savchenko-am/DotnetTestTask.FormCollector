# DotnetTestTask.FormCollector

A tool for storing form submissions, consisting of both a backend and a frontend.

## Setup

```bash
git clone https://github.com/alexey-savchenko-am/DotnetTestTask.FormCollector.git
cd DotnetTestTask.FormCollector
docker compose up --build --force-recreate
```
Ensure that ports **8080** and **3000** are available on your machine. If not, update them in **docker-compose.yml**.

- **Swagger for the backend API**: [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)  
- **Frontend**: [http://localhost:3000/](http://localhost:3000/)  
- **Client creation form**: [http://localhost:3000/clients/create/](http://localhost:3000/clients/create/)  
- **Submission list and search form**: [http://localhost:3000/submissions](http://localhost:3000/submissions)

## Part 2: Handling Large Files

Some web forms may have large files (about 100 MB). There could be many submissions, each with multiple files. Here is a simple way to handle it:

- **Where to store files**: Keep the files outside the database. Use S3, Azure Blob Storage, or a local file server. The database only stores file info: name, type, size, and link to the file.

- **Database structure**:  
  - `Submissions` table: stores the form data.  
  - `Attachments` table: stores file info and links it to a submission.  

- **API**:  
  - Upload a file: `POST /submissions/{submissionId}/attachments`  
  - List files for a submission: `GET /submissions/{submissionId}/attachments`  
  - Download a file: `GET /attachments/{attachmentId}/download`  

- **Other tips**:  
  - Use streaming or direct links to upload/download big files without overloading the server.  
  - Load file lists in pages (pagination) for speed.  
  - Optionally, use a CDN to make downloads faster.
