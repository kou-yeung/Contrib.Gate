class Random {

    // [min,max)
    static NextInteger(min: number, max: number): number {
        return Math.floor(this.NextDouble(min,max));
    }

    // [min,max)
    static NextDouble(min: number, max: number): number {
        return Math.random() * (max - min) + min;
    }
}
