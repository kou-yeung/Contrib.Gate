﻿// MEMO : KiiCloud では Arrayが使えないため extendsは 使用できません!!
// このクラスはメンバで持ちます
class Result/*<T>*/ {
    private obj: KiiObject;

    constructor(obj: KiiObject) {
        this.obj = obj;
    }

    save(done: (result: Result) => void): void {
        let self = this;
        self.obj.save({
            success: function (theSavedObject: KiiObject) {
                self.obj = theSavedObject; // 最新のほうを使う!!
                done(self);
            },
            failure: function (theObject, error) {
            }
        }, true);
    };
    get<T>(key: string, def?: T): T {
        if (def == undefined || this.has(key)) {
            return this.obj.get<T>(key);
        } else {
            return def;
        }
    }
    set<T>(key: string, value: T) {
        this.obj.set<T>(key, value);
    }
    has(key: string): boolean {
        return this.allkey.indexOf(key) != -1;
    }
    get allkey(): string[] {
        return this.obj.getKeys();
    }

    delete(done: (result: Result) => void): void {
        let self = this;
        self.obj.delete({
            success: function (theDeleteObject: KiiObject) {
                done(self);
            },
            failure: function (theObject, error) {
            }
        });
    }
}
class Bucket<T> {
    private user: KiiUser | KiiAppAdminContext;
    private t: T;
    private bucketName: string;
    results: Result[];

    constructor(t: T, user: KiiUser | KiiAppAdminContext | null, bucketName: string) {
        this.t = t;
        this.user = user;
        this.bucketName = bucketName;
    }

    private bucketWithName(): KiiBucket {
        if (this.user == null) {
            return Kii.bucketWithName(this.bucketName);
        } else {
            return this.user.bucketWithName(this.bucketName);
        }
    }

    refresh(done: (t: T) => void, query: KiiQuery = null): void {
        let self = this;

        if (query == undefined || query == null) {
            query = KiiQuery.queryWithClause();
        }

        var bucket = self.bucketWithName();

        self.results = [];

        bucket.executeQuery(query, {
            success: function (query, result, next) {
                if (result.length != 0) {
                    for (var i = 0; i < result.length; i++) {
                        self.results.push(new Result(result[i]));
                    }
                } else {
                    self.results.push(new Result(self.bucketWithName().createObject()));
                }
                done(self.t);
            },
            failure: function (query, error) {
                // エラー処理
            }
        });
    }

    get first(): Result {
        return this.results[0];
    }

    save(done: (t: T) => void): void {
        let self = this;
        let process = 0;
        for (var i = 0; i < this.results.length; i++) {
            this.results[i].save(() => {
                if (++process >= this.results.length) {
                    done(self.t);
                }
            });
        }
    };

}
