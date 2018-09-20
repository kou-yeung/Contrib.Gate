/// <summary>
/// ApiError
/// </summary>
class ApiError {
    errorCode: ErrorCode; // エラーコードが 0 以外だとエラー
    message: string;
    constructor(errorCode: ErrorCode, message?:string) {
        this.errorCode = errorCode;
        this.message = message;
    }

    Pack(): string {
        return JSON.stringify(this);
    }

    static Create(errorCode: ErrorCode, message?: string): ApiError {
        return new ApiError(errorCode, message);
    }
}
