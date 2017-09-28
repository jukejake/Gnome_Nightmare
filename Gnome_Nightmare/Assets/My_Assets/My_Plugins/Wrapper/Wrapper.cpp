#include "Wrapper.h"
#include "Windows.h"
#include <string>
#include <iostream>
#include <fstream>

char* Helloworld()
{
	return "Hello World";
}

void Log(char* a_ObjectName, char* a_Item, char* a_Value) {

	std::string ab_ObjectName = "Assets/My_Assets/My_Log/";
	ab_ObjectName.append(a_ObjectName);
	char *ac_ObjectName = &ab_ObjectName[0u];

	CreateDirectoryA(ac_ObjectName, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND) {
		std::string I_ErrorMsg = "Error creating directory.";
		I_ErrorMsg += ac_ObjectName;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);
		Out << I_ErrorMsg;
		Out.close();
		return;
	}
	std::string I_Dir(ac_ObjectName);
	I_Dir += "/";
	I_Dir += a_Item;
	I_Dir += ".txt";

	std::ofstream I_Out;
	I_Out.open(I_Dir, std::ios_base::app);
	I_Out << a_Value << std::endl;
	I_Out.close();
}

void ClearLog(char* a_ObjectName, char* a_Item) {

	std::string ab_ObjectName = "Assets/My_Assets/My_Log/";
	ab_ObjectName.append(a_ObjectName);
	char *ac_ObjectName = &ab_ObjectName[0u];

	CreateDirectoryA(ac_ObjectName, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND) {
		std::string I_ErrorMsg = "Error creating directory.";
		I_ErrorMsg += ac_ObjectName;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);
		Out << I_ErrorMsg;
		Out.close();
		return;
	}
	std::string I_Dir(ac_ObjectName);
	I_Dir += "/";
	I_Dir += a_Item;
	I_Dir += ".txt";

	std::ofstream I_Out;
	I_Out.open(I_Dir, std::ofstream::out | std::ofstream::trunc);
	I_Out.close();
}

char* LoadDialogue(char* a_ObjectName, char* a_Item) {
	std::string ab_ObjectName = "Assets/My_Assets/My_Dialogue/";
	ab_ObjectName.append(a_ObjectName);
	char *ac_ObjectName = &ab_ObjectName[0u];

	CreateDirectoryA(ac_ObjectName, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND) {
		std::string I_ErrorMsg = "Error creating directory.";
		I_ErrorMsg += ac_ObjectName;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);
		Out << I_ErrorMsg;
		Out.close();
		return "ERROR.001 Could not create directory";
	}
	std::string I_Dir(ac_ObjectName);
	I_Dir += "/";
	I_Dir += a_Item;
	I_Dir += ".txt";

	char* a_Text;
	std::fstream I_In;
	I_In.open(I_Dir, std::fstream::in);
	if(I_In.is_open()) {
		std::string line;
		std::string allLines;
		while (std::getline(I_In, line)) {
			allLines.append(line);
		}

		for (int i = allLines.length(); i < 20; i++) { allLines.append(" "); }
		
		a_Text = &allLines[0u];
	}
	else { a_Text = "ERROR.002 Could not open file."; }
	I_In.close();
	
	return a_Text;
}

void SaveDialogue(char* a_ObjectName, char* a_Item, char* a_Value) {
	std::string ab_ObjectName = "Assets/My_Assets/My_Dialogue/";
	ab_ObjectName.append(a_ObjectName);
	char *ac_ObjectName = &ab_ObjectName[0u];

	CreateDirectoryA(ac_ObjectName, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND) {
		std::string I_ErrorMsg = "Error creating directory.";
		I_ErrorMsg += ac_ObjectName;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);
		Out << I_ErrorMsg;
		Out.close();
		return;
	}
	std::string I_Dir(ac_ObjectName);
	I_Dir += "/";
	I_Dir += a_Item;
	I_Dir += ".txt";

	std::ofstream I_Out;
	I_Out.open(I_Dir, std::ofstream::out | std::ofstream::trunc);
	I_Out << a_Value << std::endl;
	I_Out.close();
}
