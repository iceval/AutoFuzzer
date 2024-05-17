#!/bin/sh

dir=./findings/default/crashes

while true; do
	if [ -d "$dir" ]; then
		break
	fi
done

cd $dir
while true; do
		for f in *:*; do
			[ -s "$f" ] && mv -- "$f" "${f//:/_}"
		done
	sleep 2
done