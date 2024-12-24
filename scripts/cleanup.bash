#!/bin/bash
cd ..

# Find and delete all 'bin' and 'obj' folders
find . -type d -name "node_modules" -exec rm -rf {} +
find . -type d -name ".vs" -exec rm -rf {} +
find . -type d -name "bin" -exec rm -rf {} +
find . -type d -name "obj" -exec rm -rf {} +

echo "All 'node_modules', '.vs', 'bin', 'obj' folders have been deleted."
