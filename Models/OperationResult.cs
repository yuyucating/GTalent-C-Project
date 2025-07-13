namespace InventorySystem.Models;

public class OperationResult<T> // 泛型
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public OperationResult(string message, T data)
    {
        Success = true;
        Message = message;
        Data = data;
    }
    
    public OperationResult(string message)
    {
        Success = false;
        Message = message;
        Data = default(T);  // null
    }
    
    // 讓使用者使用泛型, 而非直接將建構子丟給使用者
    public static OperationResult<T> SuccessResult(string message, T data)
    {
        return new OperationResult<T>(message, data);
    }

    public static OperationResult<T> ErrorResult(string message)
    {
        return new OperationResult<T>(message);
    }
}