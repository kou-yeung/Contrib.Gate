
namespace Entities {
    export class Identify {
        idWithType: number;

        get Type(): IDType {
            return ((this.idWithType >> 24) & 0xFF) as IDType;
        }
        get Id(): number {
            return this.idWithType & 0xFFFFFF;
        }

        constructor(id: number, type?: IDType) {
            if (type == undefined || type == null) {
                this.idWithType = id;
            } else {
                this.idWithType = (type as number) << 24 | id;
            }
        }

        toString(): string {
            let str = '000000' + this.Id;
            return `${IDType[this.Type]}_${str.slice(-6, -3)}_${str.slice(-3)}`;
        }
        static Parse(str: string): Identify {
            let m = str.match(/(\w+?)_(\d+)_(\d+)/);
            if (m) {
                return new Identify(IDType[m[1]], parseInt(m[2] + m[3]));
            } else {
                return new Identify(0, IDType.Unknown);
            }
        }

        static isEmpty(identify: Identify): boolean {
            return identify.idWithType == 0;
        }
        static get Empty(): Identify {
            return new Identify(0);
        }
    }
}