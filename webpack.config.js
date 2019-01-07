// Template for webpack.config.js in Fable projects
// Find latest version in https://github.com/fable-compiler/webpack-config-template

// In most cases, you'll only need to edit the CONFIG object
// See below if you need better fine-tuning of Webpack options

const CONFIG = {
    indexHtmlTemplate: "./src/index.html",
    fsharpEntry: "./src/App.fsproj",
    cssEntry: "./src/style.scss",
    outputDir: "./deploy",
    devServerPort: 8080,
    // When using webpack-dev-server, you may need to redirect some calls
    // to a external API server. See https://webpack.js.org/configuration/dev-server/#devserver-proxy
    devServerProxy: undefined,
    // Use babel-preset-env to generate JS compatible with most-used browsers.
    // More info at https://babeljs.io/docs/en/next/babel-preset-env.html
    babel: {
        presets: [
            ["@babel/preset-env", {
                "targets": {
                    "browsers": ["last 2 versions"]
                },
                "modules": false,
                "useBuiltIns": true,
            }]
        ],
    }
}

// If we're running the webpack-dev-server, assume we're in development mode
const isProduction = !process.argv.find(v => v.indexOf('webpack-dev-server') !== -1);
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

// The HtmlWebpackPlugin allows us to use a template for the index.html page
// and automatically injects <script> or <link> tags for generated bundles.
const commonPlugins = [
    new HtmlWebpackPlugin({
        filename: 'index.html',
        template: CONFIG.indexHtmlTemplate
    })
];

module.exports = {
    // In development, bundle styles together with the code so they can also
    // trigger hot reloads. In production, put them in a separate CSS file.

    // @babel/polyfill is a polyfill. If you only need to support modern browsers, you can remove it.
    entry: isProduction ? {
        app: [CONFIG.fsharpEntry, CONFIG.cssEntry]
    } : {
            app: [CONFIG.fsharpEntry],
            style: [CONFIG.cssEntry]
        },
    // Add a hash to the output file name in production
    // to prevent browser caching if code changes
    output: {
        path: path.join(__dirname, CONFIG.outputDir),
        filename: isProduction ? '[name].[hash].js' : '[name].js'
    },
    mode: isProduction ? "production" : "development",
    devtool: isProduction ? "source-map" : "eval-source-map",
    optimization: {
        // Split the code coming from npm packages into a different file.
        // 3rd party dependencies change less often, let the browser cache them.
        splitChunks: {
            cacheGroups: {
                commons: {
                    test: /node_modules/,
                    name: "vendors",
                    chunks: "all"
                }
            }
        },
    },
    // Besides the HtmlPlugin, we use the following plugins:
    // PRODUCTION
    //      - MiniCssExtractPlugin: Extracts CSS from bundle to a different file
    //      - CopyWebpackPlugin: Copies static assets to output directory
    // DEVELOPMENT
    //      - HotModuleReplacementPlugin: Enables hot reloading when code changes without refreshing
    plugins: isProduction ?
        commonPlugins.concat([
            new MiniCssExtractPlugin({ filename: 'style.css' }),
        ])
        : commonPlugins.concat([
            new webpack.HotModuleReplacementPlugin(),
        ]),
    resolve: {
        alias: {
            react: 'preact-compat',
            'react-dom': 'preact-compat'
        },
        // See https://github.com/fable-compiler/Fable/issues/1490
        symlinks: false
    },
    // Configuration for webpack-dev-server
    devServer: {
        publicPath: "/",
        port: CONFIG.devServerPort,
        proxy: CONFIG.devServerProxy,
        historyApiFallback: true,
        hot: true,
        inline: true
    },
    // - fable-loader: transforms F# into JS
    // - babel-loader: transforms JS to old syntax (compatible with old browsers)
    // - sass-loaders: transforms SASS/SCSS into JS
    // - file-loader: Moves files referenced in the code (fonts, images) into output folder
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: "fable-loader"
            },
            /* {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader' ,
                    options: CONFIG.babel
                },
            }, */
            {
                test: /\.(sass|scss|css)$/,
                use: [
                    isProduction
                        ? MiniCssExtractPlugin.loader
                        : 'style-loader',
                    'css-loader',
                    'sass-loader',
                ],
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)(\?.*)?$/,
                use: ["file-loader"]
            }
        ]
    }
};
