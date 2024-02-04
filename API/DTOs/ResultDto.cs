namespace API.DTOs
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }

        private ResultDto() { }

        public static ResultDto<T> Success(T data)
        {
            return new ResultDto<T> { IsSuccess = true, Data = data };
        }

        public static ResultDto<T> NotFound(string errorMessage)
        {
            return new ResultDto<T> { IsSuccess = false, ErrorMessage = errorMessage };
        }

        public static ResultDto<T> BadRequest(string errorMessage)
        {
            return new ResultDto<T> { IsSuccess = false, ErrorMessage = errorMessage };
        }
    }
}
