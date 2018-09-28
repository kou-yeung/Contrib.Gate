// 範囲
namespace Entities {
    export class Range {
        start: number = NaN;
        end: number = NaN;
        static Parse(value: string | number): Range {
            let res = new Range();

            if (typeof value == "number") {
                res.end = res.start = value;
            } else if (typeof value == "string") {
                let data = value.split("~");
                if (data.length >= 2) {
                    res.start = parseInt(data[0]);
                    res.end = parseInt(data[1]);
                }
            }
            return res;
        }
        get vaild(): boolean {
            return this.start != NaN && this.end != NaN;
        }
    }
}