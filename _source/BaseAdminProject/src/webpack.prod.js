const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');
const TerserPlugin = require('terser-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const PurgeCSSPlugin = require('purgecss-webpack-plugin')
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const glob = require("glob");
const path = require('path');

module.exports = merge(common, {
    mode: 'production',
    devtool: false,
    stats: { modules: false },
    optimization: {
        minimize: true,
        minimizer: [
            new TerserPlugin({
                parallel: true
            }),
            new OptimizeCSSAssetsPlugin()
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css",
            chunkFilename: "[id].css"
        }),
        new PurgeCSSPlugin({
            paths: glob.sync(`${PATHS.src}/**/*`, { nodir: true }),
        })
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
