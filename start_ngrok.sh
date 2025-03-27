#!/bin/bash

# Default port
DEFAULT_PORT=5258

# Use provided port if given, otherwise use default
PORT=${1:-$DEFAULT_PORT}

# Start ngrok
ngrok http --url=grubworm-true-positively.ngrok-free.app $PORT

