/// <summary>
/// ApiError
/// </summary>
class ApiError {
    errorCode: ErrorCode;// // エラーコードが 0 以外だとエラー

    constructor(errorCode: ErrorCode) {
        this.errorCode = errorCode;
    }

    Pack(): string {
        return JSON.stringify(this);
    }

    static Create(errorCode: ErrorCode): ApiError {
        return new ApiError(errorCode);
    }
}
