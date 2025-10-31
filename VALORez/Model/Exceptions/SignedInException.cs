namespace Model.Exceptions;

public class SignedInException(string message = "") : ValorezException(message);