#!/bin/sh

sharpfuzz out/"$TARGET_DLL"

if [ -z "$TARGET_DICTIONARY" ]; then
	./afl/afl-fuzz \
		-i testcases/"$TARGET_TEST_METHOD_NAME" \
		-o findings \
		-t 10000 \
		-m 10000 \
		out/FuzzerApp Main/"$TARGET_TEST_DLL" "$TARGET_TEST_CLASS_NAME" "$TARGET_TEST_METHOD_NAME"
else
	./afl/afl-fuzz \
		-i testcases/"$TARGET_TEST_METHOD_NAME" \
		-o findings \
		-t 10000 \
		-m 10000 \
		-x dictionaries/"$TARGET_TEST_METHOD_NAME"/"$TARGET_DICTIONARY" \
		out/FuzzerApp Main/"$TARGET_TEST_DLL" "$TARGET_TEST_CLASS_NAME" "$TARGET_TEST_METHOD_NAME" # "$TARGET_DLL" "$TARGET_FUNCTION" - dotnet args
fi
