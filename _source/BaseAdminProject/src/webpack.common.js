"use strict";
const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const webpack = require('webpack');
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");
const StylelintPlugin = require('stylelint-webpack-plugin');
const ESLintPlugin = require('eslint-webpack-plugin');
var fs = require('fs');
var appBasePath = './scripts/';

var jsEntries = {};
// We search for index.js files inside basePath folder and make those as entries
fs.readdirSync(appBasePath).forEach(function (name) {
    var indexFile = `${appBasePath}${name}/index.js`;
    if (fs.existsSync(indexFile)) {
        jsEntries[name] = indexFile;
    }
});

module.exports = {
    entry: jsEntries,
    plugins: [
        new CleanWebpackPlugin(),
        new webpack.ProvidePlugin({
            $: require.resolve('jquery'),
            jQuery: require.resolve('jquery'),
            "window.jQuery": "jquery"
        }),
        new FixStyleOnlyEntriesPlugin(),
        new StylelintPlugin(),
        new ESLintPlugin()
    ],
    output: {
        filename: 'js/[name].bundle.js',
        path: path.resolve(__dirname, '../wwwroot/dist'),
        publicPath: '/'
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                use: [
                    {
                        loader: "style-loader",
                        options: {
                            minimize: true || {/* or CSSNano Options */ }
                        }
                    },
                    {
                        loader: "css-loader",
                        options: {
                            minimize: true || {/* or CSSNano Options */ }
                        }
                    }
                ]
            },
            {
                test: /\.js$/,
                loader: 'babel-loader',
                options: {
                    "presets": [
                        [
                            "@babel/preset-env",
                            {
                                "modules": "commonjs",
                                "targets": {
                                    "node": "current",
                                    "ie": "11"
                                }
                            }
                        ]
                    ]
                }
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2|otf)$/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        name: 'fonts/[hash]-[name].[ext]'
                    }
                }]
            },
            {
                test: /\.(png|jp(e*)g|svg)$/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 8000, // Convert images < 8kb to base64 strings
                        name: '../img/[hash]-[name].[ext]'
                    }
                }]
            }
        ]
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js"]
    },
    optimization: {
        splitChunks: {
            chunks: "all",
            minSize: 0,
            cacheGroups: {
                vendors: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendors',
                    priority: -10,
                    chunks: "all",
                    enforce: true
                },
                default: {
                    name: 'commonChunks',
                    minChunks: 2,
                    priority: -20,
                    reuseExistingChunk: true
                }
            }
        }
    }

};
