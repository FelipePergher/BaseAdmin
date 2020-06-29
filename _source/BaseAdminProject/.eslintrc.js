module.exports = {
    "env": {
        "browser": true,
        "es2020": true,
        "commonjs": true,
        "jquery": true
    },
    "extends": "eslint:recommended",
    "parserOptions": {
        "ecmaVersion": 11,
        "sourceType": "module"
    },
    "rules": {
        "semi": ["error", "always"],
        "indent": ["error", 4, { "SwitchCase": 1 }]
    }
};
