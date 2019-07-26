@echo off
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" CommonModelMessage.proto
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" GatherOuterMessage.proto  
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" UserMessage.proto
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" LobbyMessage.proto
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" MatchMessage.proto 
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" FriendsCircleMessage.proto 
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" JoyLandlordsMessage.proto  
protoc.exe --csharp_out="./Assets/Hotfix/Module/Message/" --proto_path="../Proto/" CardFiveStarMessage.proto 
echo finish... 
pause