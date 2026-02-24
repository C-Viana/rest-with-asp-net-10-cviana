
### REF DOC WITH DOCKER COMMANDS USED ON THIS PROJECT

REPOSITORY_NAME="cedsv"
IMAGE_NAME="asp10-api"
IMAGE_VERSION="0.0.1"

# Build a new docker image
docker build -t $IMAGE_NAME:$IMAGE_VERSION -f .

# Tagging image with repository name
docker tag $IMAGE_NAME:$IMAGE_VERSION $REPOSITORY_NAME/$IMAGE_NAME:$IMAGE_VERSION

# Sign in on Docker
docker login docker.io

# Upload image to Docker Hub
docker push $REPOSITORY_NAME/$IMAGE_NAME:$IMAGE_VERSION

# View running conainers information
docker container ls

# View last logs of one specific container
docker container logs {CONTAINER_ID}

# Open/follows real time logs for the specified container
docker container logs {CONTAINER_ID} -f

# Run compose file in detached mode after building the specified app image
docker compose up -d --build