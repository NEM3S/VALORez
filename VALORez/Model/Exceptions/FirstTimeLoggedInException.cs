namespace Model.Exceptions;

public class FirstTimeLoggedInException(string message = "") : ValorezException(message);