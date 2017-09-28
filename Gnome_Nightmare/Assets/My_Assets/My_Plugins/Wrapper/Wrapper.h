#pragma once
#include "LibSettings.h"


#ifdef __cplusplus
extern "C"
{
#endif
	LIB_API char*Helloworld();
	LIB_API void Log(char* ObjectName, char* Item, char* Value);
	LIB_API void ClearLog(char* ObjectName, char* Item);
	LIB_API char*LoadDialogue(char* ObjectName, char* Item);
	LIB_API void SaveDialogue(char* ObjectName, char* Item, char* Value);
#ifdef __cplusplus
}
#endif