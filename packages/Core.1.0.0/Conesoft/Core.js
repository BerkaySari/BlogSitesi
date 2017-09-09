//daves mini jquery, because that's how i rule
HTMLElement.prototype.appendHTML = function (content) {
    var el = document.createElement('div');
    this.appendChild(el);
    el.outerHTML = content;
    return this.lastElementChild;
};

HTMLElement.prototype.remove = function () {
    this.parentElement.removeChild(this);
};

//IE9 fix
if (!window.console) window.console = {};
if (!window.console.log) window.console.log = function () { };

var Sandbox = function (id, core) {
    "use strict";
    this.id = id;
    this.log = core.log;
    this.subscribe = core.subscribe.bind(core);
    this.publish = core.publish.bind(core);
    this.html = function (content, model) {
        core.updateDOM(id, content, model);
    }.bind(this);
    this.append = function (child, model) {
        core.appendToDOM(this.id, child, model);
    }.bind(this);
    this.create = function (type) {
        return core.createDOMElement(type);
    }.bind(this);
    this.children = function () {
        return core.getDOMChildren(id);
    }.bind(this);
    this.observable = core.observable.bind(this);
    this.content = core.content;
    this.appendHTML = function (content, model) {
        return core.appendHTMLToDOM(this.id, content, model);
    }.bind(this);
};

var Core = function () {
    "use strict";
    var modules = {},
        constructors = {},
        cache = {},
        content = {};

    var argsof = function (array, shift) {
        var args = Array.prototype.slice.call(array, shift);
        args = args === undefined ? [] : args;
        args = args instanceof Array === false ? [args] : args;
        return args;
    };

    var log = function () {
        var indend = 0;
        return {
            enabled: false,
            println: function (text) {
                if (this.enabled === true) {
                    console.log(new Array(indend + 1).join('\t') + text);
                }
            },
            print: function (text) {
                this.println(text + ';');
            },
            begin: function (text) {
                this.println(text + '(' + argsof(arguments, 1).join(', ') + ') {');
                ++indend;
            },
            end: function () {
                --indend;
                this.println('}');
            }
        };
    }();

    var getContent = function (from) {
        var content = {}, elements = from.childNodes;

        for (var i = 0, length = elements.length; i < length; ++i) {
            var element = elements[i];
            if (element.nodeType === 1) {
                content[element.id] = element.outerHTML.trim();
            }
        }

        return content;
    };

    return {
        log: log,

        modules: modules,

        content: content,

        user: undefined,

        start: function (id) {
            log.begin('Core.start', id);
            this.stop(id);
            if (modules[id] instanceof Function) {
                constructors[id] = modules[id];
                modules[id] = modules[id](new Sandbox(id, this));
            }
            log.end();
        },

        stop: function (id) {
            var module = modules[id];
            if (module instanceof Function === false) {
                log.begin('Core.stop');
                module.destroy();
                module = constructors[id];
                log.end();
            }
        },

        startAll: function () {
            log.begin('Core.startAll');
            for (var id in modules) {
                if (modules.hasOwnProperty(id)) {
                    this.start(id);
                }
            }
            log.end();
            this.publish('init');
            run.now(function () { this.publish('initdone'); }.bind(this));
        },

        stopAll: function () {
            log.begin("Core.stopAll");
            for (var id in modules) {
                if (modules.hasOwnProperty(id)) {
                    this.stop(id);
                }
            }
            log.end();
        },

        setup: function (options) {
            this.content = getContent(options.content);
            return this;
        },

        publish: function (message) {
            try {
                var args = argsof(arguments, 1);
                Object.keys(modules).forEach(function (modulename) {
                    var module = modules[modulename];
                    Object.keys(module).forEach(function (methodname) {
                        if (methodname === 'on' + message || (methodname === 'init' && message === 'init')) {
                            log.begin(modulename + '.' + message, args);
                            module[methodname].apply(this, args);
                            log.end();
                        }
                    });
                });
            } catch (err) {
                console.log(err);
            }
        },

        subscribe: function(message, callback) {
            if (!cache[message]) {
                cache[message] = [];
            }
            cache[message].push(callback);
            return [message, callback];
        },

        updateDOM: function (id, content, model) {
            log.begin('Core.updateDOM');
            var element = document.getElementById(id);
            if (element !== undefined) {
                element.innerHTML = content;
            }
            if (model !== undefined) {
                ko.applyBindings(model, element);
            }
            log.end();
        },

        appendToDOM: function(id, child, model) {
            document.getElementById(id).appendChild(child);
            if (model !== undefined) {
                ko.applyBindings(model, child);
            }
        },

        appendHTMLToDOM: function (id, content, model) {
            var element = document.getElementById(id).appendHTML(content);
            if (model !== undefined) {
                ko.applyBindings(model, element);
            }
            return element;
        },

        createDOMElement: function(type) {
            return document.createElement(type);
        },

        getDOMChildren: function(id) {
            return document.getElementById(id).childNodes;
        },

        observable: function (value) {
            if (value instanceof Array) {
                return ko.observableArray(value);
            }
            return ko.observable();
        }
    };    
}();
