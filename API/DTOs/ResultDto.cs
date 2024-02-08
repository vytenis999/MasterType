namespace API.DTOs
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; } = false;
        public bool IsNotFound { get; set; } = false;
        public bool IsBadRequest { get; set; } = false;
        public bool IsUnauthorized { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public static ResultDto Success(string successMessage)
        {
            return new ResultDto { IsSuccess = true, SuccessMessage = successMessage };
        }

        public static ResultDto NotFound(string errorMessage)
        {
            return new ResultDto { IsNotFound = true, ErrorMessage = errorMessage };
        }

        public static ResultDto BadRequest(string errorMessage)
        {
            return new ResultDto { IsBadRequest = true, ErrorMessage = errorMessage };
        }

        public static ResultDto Unauthorized()
        {
            return new ResultDto { IsBadRequest = true };
        }
    }

    public class ResultDto<T> : ResultDto
    {
        public T Data { get; private set; }

        public static ResultDto<T> Success(T data)
        {
            return new ResultDto<T> { IsSuccess = true, Data = data };
        }

        public static new ResultDto<T> NotFound(string errorMessage)
        {
            return new ResultDto<T> { IsNotFound = true, ErrorMessage = errorMessage };
        }

        public static new ResultDto<T> BadRequest(string errorMessage)
        {
            return new ResultDto<T> { IsBadRequest = true, ErrorMessage = errorMessage };
        }
        public static new ResultDto<T> Unauthorized()
        {
            return new ResultDto<T> { IsBadRequest = true };
        }
    }
}
