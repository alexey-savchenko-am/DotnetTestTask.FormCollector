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
