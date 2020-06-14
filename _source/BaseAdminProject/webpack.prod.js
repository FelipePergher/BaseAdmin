const merge = require('webpack-merge');
const common = require('./webpack.common.js');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const glob = require("glob");
const path = require('path');

module.exports = merge(common, {
    mode: 'production',
    devtool: false,
    stats: { modules: false },
    optimization: {
        minimizer: [
            new UglifyJsPlugin({
                cache: true,
                parallel: true,
                sourceMap: true // set to true if you want JS source maps
            }),
        ]
    },
    plugins: [
    ],
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: 'css/[folder].bundle.css'

                        }
                    },
                    {
                        loader: 'extract-loader'
                    },
                    {
                        loader: "css-loader",
                        options: {
                            sourceMap: false
                        }
                    },
                    {
                        loader: "sass-loader",
                        options: {
                            sassOptions: {
                                includePaths: glob.sync('node_modules').map((d) => path.join(__dirname, d))
                            }
                        }
                    }
                ]
            }
        ]
    }
});