# 2opt.NET

A simple test application that generates an N-point polygon consisting of random points, then applies the [2-opt](https://en.wikipedia.org/wiki/2-opt) algorithm to mutate intersecting lines, resulting in a polygon of roughly the same shape with a contiguous inner area.

## Output

Produces spatial SQL queries, allowing the original and 2-opted polygon to be plotted in SQL Server Management Studio (paste into a query window and execute, then flip to the Spatial Results tab).
