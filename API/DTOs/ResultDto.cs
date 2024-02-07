namespace API.DTOs
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; private set; } = false;
        public bool IsNotFound { get; private set; } = false;
        public bool IsBadRequest { get; private set; } = false;
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }

        private ResultDto() { }

        public static ResultDto<T> Success(T data)
        {
            return new ResultDto<T> { IsSuccess = true, Data = data };
        }

        public static ResultDto<T> NotFound(string errorMessage)
        {
            return new ResultDto<T> { IsNotFound = true, ErrorMessage = errorMessage };
        }

        public static ResultDto<T> BadRequest(string errorMessage)
        {
            return new ResultDto<T> { IsBadRequest = true, ErrorMessage = errorMessage };
        }
    }
}
