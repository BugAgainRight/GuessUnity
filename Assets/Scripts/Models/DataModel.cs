using System;
using System.Collections.Generic;

[Serializable]
public class RegisterData
{
    public string Account;
    public string Password;
    public string IDNumber; // 身份证ID
}

[Serializable]
public class StatusData
{
    public bool Success;
    public string Message;
}

[Serializable]
public class LoginData
{
    public string Account;
    public string Password;
}

[Serializable]
public class LoginResponse
{
    public string ID; // 注册时后端给用户分配ID，登陆时返回
    public bool Success;
    public string Message;
}

[Serializable]
public class UserInfo
{
    public string Account;
    public string Name;
    public string PhoneNumber;
    public string Address;
    public double Points;
}

[Serializable]
public class UpdateInfoRequest
{
    public string Account;
    public string Name;
    public string PhoneNumber;
    public string Address;
}

[Serializable]
public class EventList
{
    public List<Event> Events = new();
}

[Serializable]
public class Event
{
    public string ID;
    public string Name;	// 比赛名称
    public DateTime EventTime, StartGuessTime, EndGuessTime;
    public string[] PartyANames, PartyBNames; // 队伍A的所有成员的名字，队伍B的所有成员的名字（如果是单人则单人）
    public string PartyACountry, PartyBCountry; // 国家A的名称，国家B的名称
    public int Winner; // 获胜方，如果在竞赛结束之前，则返回-1，竞猜结束后返回0或者1
}

[Serializable]
public class GuessList
{
    public List<GuessData> Guesses = new();
}

[Serializable]
public class GuessData
{
    public string EventID;
    public int GuessWinner;
}

[Serializable]
public class TimeData
{
    public DateTime ServerTime;
}

[Serializable]
public class EventGuessData
{
    public int[] GuessCount;
    public int UserGuess;
}

[Serializable]
public class PrizeList{
    public List<Prize> Prizes = new();
}

[Serializable]
public class Prize{
    public string ID;
    public string Name;
    public int Stock;//剩余库存数量
    public double PointsRequired;//兑换一件所需要的积分
    public bool Redeemed;//true表示用户已兑换，false表示用户未兑换
}

[Serializable]
public class MessageList{
    public List<MessageData> Messages = new();
}
    
[Serializable]
public class MessageData{
    public string ID = Guid.NewGuid().ToString();
    public string Content;
    public DateTime Time;
}

[Serializable]
public class MessageReadList
{
    public List<string> ReadMessages = new();
}
