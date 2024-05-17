#!/bin/sh

set -m
  
/bin/bash ./run.sh &
  
/bin/bash ./rename.sh

fg %1