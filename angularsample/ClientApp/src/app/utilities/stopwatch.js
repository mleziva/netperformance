"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Stopwatch = /** @class */ (function () {
    function Stopwatch() {
    }
    Stopwatch.prototype.start = function () {
        this.timestart = new Date().getTime();
    };
    Stopwatch.prototype.stop = function () {
        this.elapsed = (new Date().getTime() - this.timestart);
    };
    Stopwatch.prototype.clear = function () {
        this.timestart = null;
        this.elapsed = null;
    };
    return Stopwatch;
}());
exports.Stopwatch = Stopwatch;
//# sourceMappingURL=stopwatch.js.map