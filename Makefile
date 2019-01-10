DEFAULT_GOAL: init

## clear bin & obj
clean-up-compiled:
	find . -iname "bin" -o -iname "obj" | xargs rm -rf

## paket update
paket-update:
	mono .paket/paket.exe update

## install deps with paket
paket-deps:
	mono .paket/paket.exe install && \
	mono .paket/paket.exe restore

## builds project
build: 
	dotnet build .

help: SHELL:=/bin/bash
## Prints this help
help:
	@echo -e "\nUsage: make <target>\n\nThe following targets are available:\n";
	@while IFS='' read -r line || [[ -n "$$line" ]]; \
	do \
		if [ "$$HELP_TEXT" != "" ]; \
		then \
			TARGET=`echo $$line | sed 's/\(^.*\):.*/\1/'`; \
			printf "\e[1;34m%-30s\e[m %s\n" "$$TARGET" "$$HELP_TEXT"; \
		fi; \
		HELP_TEXT=`echo $$line | sed 's/^##\(.*\)/\1/'`; \
		if [ "`echo -e $$HELP_TEXT`" == "`echo -e $$line`" ]; \
		then \
			HELP_TEXT=""; \
		fi; \
	done < $(MAKEFILE_LIST)
