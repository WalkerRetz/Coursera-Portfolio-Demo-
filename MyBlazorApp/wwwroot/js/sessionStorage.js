window.eventEaseSession = {
    set: function (key, value) {
        try {
            localStorage.setItem(key, JSON.stringify(value));
        } catch (e) { }
    },
    get: function (key) {
        try {
            var v = localStorage.getItem(key);
            return v ? JSON.parse(v) : null;
        } catch (e) { return null; }
    },
    remove: function (key) {
        try { localStorage.removeItem(key); } catch (e) { }
    }
};
