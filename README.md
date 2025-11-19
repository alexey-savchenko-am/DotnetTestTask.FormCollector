# DotnetTestTask.FormCollector

A tool for storing form submissions. It consists of a backend as well as a frontend.

# Setup

```bash
git clone https://github.com/alexey-savchenko-am/DotnetTestTask.FormCollector.git
cd DotnetTestTask.FormCollector
docker compose up --build --force-recreate
```
Make sure ports 8080 and 3000 on your machine are available. Otherwise, change them in **docker-compose.yml**.

- **Swagger for the backend API** is available at: [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)  
- **Frontend** is available at: [http://localhost:3000/](http://localhost:3000/)  
- **Client creation form**: [http://localhost:3000/clients/create/](http://localhost:3000/clients/create/)  
- **Submission list and search form**: [http://localhost:3000/submissions](http://localhost:3000/submissions)


