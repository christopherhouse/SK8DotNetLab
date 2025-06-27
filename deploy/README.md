# Container Deployment Guide

This guide covers how to containerize and deploy the SK8DotNet solution to Azure Container Apps.

## 🐳 Containerization

Both projects (`SK8DotNet.Chat` and `SK8DotNet.API`) have been containerized with Docker.

### Building Docker Images

```bash
# Build the Chat app
cd SK8DotNet.Chat
docker build -t sk8dotnet-chat .

# Build the API
cd SK8DotNet.API
docker build -t sk8dotnet-api .
```

### Local Development with Docker Compose

Run both applications locally using Docker Compose:

```bash
# From solution root
docker-compose up --build
```

This will start:
- Chat app on http://localhost:5000
- API on http://localhost:5001

## ☁️ Azure Container Apps Deployment

### Prerequisites

1. Azure CLI installed and logged in
2. Azure Container Registry (ACR) set up
3. Azure Container Apps Environment created

### Step 1: Push Images to Azure Container Registry

```bash
# Tag images for ACR
docker tag sk8dotnet-chat your-registry.azurecr.io/sk8dotnet-chat:latest
docker tag sk8dotnet-api your-registry.azurecr.io/sk8dotnet-api:latest

# Push to ACR
docker push your-registry.azurecr.io/sk8dotnet-chat:latest
docker push your-registry.azurecr.io/sk8dotnet-api:latest
```

### Step 2: Deploy to Azure Container Apps

Using the provided deployment files:

```bash
# Update the YAML files with your specific values:
# - resourceGroup
# - managedEnvironmentId  
# - registry server
# - identity resource ID
# - image names

# Deploy Chat app
az containerapp create \
  --resource-group <your-rg> \
  --environment <your-env> \
  --yaml deploy/chat-containerapp.yaml

# Deploy API
az containerapp create \
  --resource-group <your-rg> \
  --environment <your-env> \
  --yaml deploy/api-containerapp.yaml
```

### Step 3: Configure Environment Variables

Both apps support these environment variables:

- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployments
- `ASPNETCORE_URLS`: Configured to listen on port 8080 for Azure Container Apps
- Application Insights connection strings (if using)

## 🔧 Configuration Notes

### Port Configuration

- Both containers expose port **8080** (Azure Container Apps standard)
- Local development uses ports 5000 (Chat) and 5001 (API)

### Security

- Containers run as non-root user `appuser`
- Minimal attack surface with multi-stage builds
- Only runtime dependencies included in final image

### Scaling

- Chat app: 1-10 replicas, scales on HTTP requests (100 concurrent max)
- API: 1-5 replicas, scales on HTTP requests (50 concurrent max)

### Resource Limits

**Chat App:**
- CPU: 0.5 cores
- Memory: 1GB

**API:**
- CPU: 0.25 cores  
- Memory: 0.5GB

## 🚀 CI/CD Integration

The Docker setup is ready for integration with:

- GitHub Actions
- Azure DevOps Pipelines
- Azure Container Registry Tasks

Example GitHub Action workflow can be added to build and deploy automatically on commits.

## 📋 Deployment Checklist

- [ ] Azure Container Registry created
- [ ] Azure Container Apps Environment created
- [ ] Docker images built and tested locally
- [ ] Images pushed to ACR
- [ ] Deployment YAML files updated with your values
- [ ] Container Apps deployed
- [ ] DNS/Custom domains configured (if needed)
- [ ] Application Insights configured (if needed)
- [ ] Monitoring and logging set up