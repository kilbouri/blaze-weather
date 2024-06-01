/**
 * Calls preventDefault for the following keys:
 * - Arrow Up
 * - Arrow Down
 * 
 * There is probably a better way to do this but tbh I don't care. I just
 * want this thing to work.
 * 
 * @param {KeyboardEvent} e the event
 */
const preventDefaultForSearchInputs = (e) => {
    if (["ArrowUp", "ArrowDown"].includes(e.code)) {
        e.preventDefault();
    }
}

/**
 * Adds the required listeners to `elementReference` to polish up
 * client-side behaviour of a search field where needed.
 * 
 * @param {HTMLElement} elementReference 
 */
export const registerSearchField = (elementReference) => {
    console.debug(`Registering search field`);
    elementReference.addEventListener("keydown", preventDefaultForSearchInputs);
    }
    
    /**
     * Removes the required listeners to `elementReference` to un-polish up
     * client-side behaviour of a search field. Inverse of `registerSearchField`.
     * 
     * @param {HTMLElement} elementReference 
     * 
     * @see registerSearchField
    */
   export const unregisterSearchField = (elementReference) => {
    console.debug(`Unregistering search field`);
    elementReference.removeEventListener("keydown", preventDefaultForSearchInputs);
}
