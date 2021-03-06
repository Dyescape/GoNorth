(function(GoNorth) {
    "use strict";
    (function(DefaultNodeShapes) {
        (function(Actions) {

            /// Action Type for changing the player value
            var actionTypeChangePlayerValue = 1;

            /**
             * Change player value Action
             * @class
             */
            Actions.ChangePlayerValueAction = function()
            {
                GoNorth.DefaultNodeShapes.Actions.ChangeValueAction.apply(this);
            };

            Actions.ChangePlayerValueAction.prototype = jQuery.extend({ }, GoNorth.DefaultNodeShapes.Actions.ChangeValueAction.prototype);

            /**
             * Builds the action
             * 
             * @returns {object} Action
             */
            Actions.ChangePlayerValueAction.prototype.buildAction = function() {
                return new Actions.ChangePlayerValueAction();
            };

            /**
             * Returns the type of the action
             * 
             * @returns {number} Type of the action
             */
            Actions.ChangePlayerValueAction.prototype.getType = function() {
                return actionTypeChangePlayerValue;
            };

            /**
             * Returns the label of the action
             * 
             * @returns {string} Label of the action
             */
            Actions.ChangePlayerValueAction.prototype.getLabel = function() {
                return DefaultNodeShapes.Localization.Actions.ChangePlayerValueLabel;
            };

            /**
             * Returns the object type name. Used for dependency objects
             * 
             * @returns {string} Object Type name used for depends on objects 
             */
            Actions.ChangePlayerValueAction.prototype.getObjectTypeName = function() {
                return Actions.RelatedToObjectNpc;
            };

            /**
             * Returns the object resource
             * 
             * @returns {int} Object Resource
             */
            Actions.ChangePlayerValueAction.prototype.getObjectId = function() {
                return "PlayerNpc";
            };

            /**
             * Returns the object resource
             * 
             * @returns {int} Object Resource
             */
            Actions.ChangePlayerValueAction.prototype.getObjectResource = function() {
                return GoNorth.DefaultNodeShapes.Shapes.ObjectResourceNpc;
            };

            /**
             * Loads the npc
             * 
             * @returns {jQuery.Deferred} Deferred for the npc loading
             */
            Actions.ChangePlayerValueAction.prototype.loadObject = function() {
                var def = new jQuery.Deferred();

                var self = this;
                GoNorth.HttpClient.get("/api/KortistoApi/PlayerNpc").done(function(data) {
                    def.resolve(data);
                }).fail(function(xhr) {
                    def.reject();
                });

                return def.promise();
            };

            GoNorth.DefaultNodeShapes.Shapes.addAvailableAction(new Actions.ChangePlayerValueAction());

        }(DefaultNodeShapes.Actions = DefaultNodeShapes.Actions || {}));
    }(GoNorth.DefaultNodeShapes = GoNorth.DefaultNodeShapes || {}));
}(window.GoNorth = window.GoNorth || {}));