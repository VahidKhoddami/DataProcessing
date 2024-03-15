# Build stage (installs dependencies)
FROM ubuntu:23.04 AS builder

# avoid stuck build due to user prompt
ARG DEBIAN_FRONTEND=noninteractive

# Update package lists and install dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        python3.11 python3.11-dev python3.11-venv python3-pip python3-wheel build-essential &&\
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# create and activate virtual environment
# using final folder name to avoid path issues with packages
RUN python3.11 -m venv /home/myuser/venv
ENV PATH="/home/myuser/venv/bin:$PATH"

COPY requirements.txt .

# Install pytorch
RUN pip install --no-cache-dir torch torchvision --index-url https://download.pytorch.org/whl/cpu
    
# Install any needed packages specified in requirements.txt
RUN pip install --no-cache-dir -r requirements.txt 


# Final image stage (copies application and runs)
FROM ubuntu:23.04 AS runner-image

# Update package lists and install dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        python3.11 python3.11-venv\
        # Dependencies for gobject and weasyprint
        libcairo2-dev libpango1.0-dev libjpeg-dev libharfbuzz-dev librsvg2-dev \
        libgl1-mesa-glx \
        # Install gobject
        gobject-introspection && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

RUN useradd --create-home myuser
COPY --from=builder /home/myuser/venv /home/myuser/venv

USER myuser
RUN mkdir /home/myuser/code
WORKDIR /home/myuser/code

# Copy additional files
COPY main.py .

# make sure all messages always reach console
ENV PYTHONUNBUFFERED=1

# activate virtual environment
ENV VIRTUAL_ENV=/home/myuser/venv
ENV PATH="/home/myuser/venv/bin:$PATH"

# Start the app using serve command
CMD [ "python", "main.py" ]