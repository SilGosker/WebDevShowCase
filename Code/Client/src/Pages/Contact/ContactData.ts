export function updateValidity(
    object: { value: string | undefined; valid: boolean },
    value: string | number,
    valid: boolean,
) {
    object.value = value as string;
    object.valid = valid;
}

export const firstName = {
    value: undefined,
    valid: false,
};

export const lastName = {
    value: undefined,
    valid: false,
};

export const email = {
    value: undefined,
    valid: false,
};

export const phone = {
    value: undefined,
    valid: false,
};

export const subject = {
    value: undefined,
    valid: false,
};

export const message = {
    value: undefined,
    valid: false,
};