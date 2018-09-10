
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
            let n1 = ('000' + Math.floor(this.Id / 1000)).slice(-3);
            let n2 = ('000' + Math.floor(this.Id % 1000)).slice(-3);
            return `${IDType[this.Type]}_${n1}_${n2}`;
        }
    }
}